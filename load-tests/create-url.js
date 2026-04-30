import http from "k6/http";
import { sleep, check } from "k6";

const BASE_URL = __ENV.BASE_URL || "https://localhost:7032";

export const options = {
    stages: [
        { duration: "30s", target: 20 },
        { duration: "1m", target: 20 },
        { duration: "15s", target: 0 },
    ],
    thresholds: {
        http_req_failed: ["rate<0.01"],
        http_req_duration: ["p(95)<500"],
    },
};

export function setup() {
    const res = http.post(
        `${BASE_URL}/api/login`,
        JSON.stringify({ email: "test@example.com", password: "abc" }),
        { headers: { "Content-Type": "application/json" } },
    );

    check(res, { "login succeeded": (r) => r.status === 200 });

    return { token: res.json("accessToken") };
}

export default function ({ token }) {
    const res = http.post(
        `${BASE_URL}/api/urls`,
        JSON.stringify({ url: "https://example.com" }),
        {
            headers: {
                "Content-Type": "application/json",
                Authorization: `Bearer ${token}`,
            },
        },
    );

    check(res, { "url shortened": (r) => r.status === 201 });

    sleep(1);
}
