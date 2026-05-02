/* eslint-disable no-undef */
import http from "k6/http";
import { sleep, check } from "k6";

export const options = {
    stages: [
        { duration: "30s", target: 50 }, // ramp up
        { duration: "1m", target: 50 }, // steady state
        { duration: "15s", target: 200 }, // sudden spike (someone tweets your link)
        { duration: "30s", target: 200 }, // hold spike
        { duration: "15s", target: 50 }, // back to normal
        { duration: "30s", target: 0 }, // ramp down
    ],
    thresholds: {
        http_req_duration: ["p(95)<50", "p(99)<100"],
        http_req_failed: ["rate<0.1"],
    },
};

const BASE_URL = __ENV.BASE_URL || "https://localhost:7032";
const POOL_SIZE = 1000;
const SHORT_CODES = Array.from(
    { length: POOL_SIZE },
    (_, i) => `test_${i + 1}`,
);

export default function () {
    const shortCode =
        SHORT_CODES[Math.floor(Math.random() * SHORT_CODES.length)];
    const res = http.get(`${BASE_URL}/${shortCode}`, { redirects: 0 });

    check(res, {
        "short URL resolved and got 302": (res) => res.status === 302,
    });

    sleep(1);
}
