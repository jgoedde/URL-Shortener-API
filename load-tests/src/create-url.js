import http from "k6/http";
import { sleep, check } from "k6";
import {generateUrlToShorten} from "./generators/domain";

const BASE_URL = __ENV.BASE_URL || "https://localhost:7032";
const TEST_USER_EMAIL = __ENV.BASE_URL || "test@example.com";
const TEST_USER_PASSWORD = __ENV.BASE_URL || "abc";

export const options = {
    stages: [
        { duration: "30s", target: 20 },
        { duration: "1m", target: 20 },
        { duration: "15s", target: 0 },
    ],
    thresholds: {
        http_req_failed: ["rate<0.01"],
        http_req_duration: ["p(99)<500", "p(95)<300"],
    },
};

export function setup() {
    const res = http.post(
        `${BASE_URL}/api/login`,
        JSON.stringify({
            email: TEST_USER_EMAIL,
            password: TEST_USER_PASSWORD,
        }),

        { headers: { "Content-Type": "application/json" } },
    );

    check(res, { "login succeeded": (r) => r.status === 200 });

    return { token: res.json("accessToken") };
}

export default function ({ token }) {
    const res = http.post(
        `${BASE_URL}/api/urls`,
        JSON.stringify({ url: generateUrlToShorten() }),
        {
            headers: {
                "Content-Type": "application/json",
                Authorization: `Bearer ${token}`,
            },
        },
    );

    check(res, {
        "is URL shortened": (r) => r.status === 201,
        "is server error": (r) => r.status >= 500 && r.status < 600,
    });

    sleep(1);
}
