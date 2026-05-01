/* eslint-disable no-undef */
import http from "k6/http";
import { sleep, check } from "k6";

export const options = {
    vus: 10,
    duration: "5s",
    // duration: "1m30s",
    thresholds: {
        http_req_duration: ["p(95)<50", "p(99)<100"],
        http_req_failed: ["rate<0.1"],
    },
};

const BASE_URL = __ENV.BASE_URL || "https://localhost:7032";
const TEST_USER_EMAIL = __ENV.TEST_USER_EMAIL || "test@example.com";
const TEST_USER_PASSWORD = __ENV.TEST_USER_PASSWORD || "abc";

/**
 * @returns {Promise<{token: string}>}
 */
async function login() {
    const res = http.post(
        `${BASE_URL}/api/login`,
        JSON.stringify({
            email: TEST_USER_EMAIL,
            password: TEST_USER_PASSWORD,
        }),

        { headers: { "Content-Type": "application/json" } },
    );

    check(res, { "login with test user succeeded": (r) => r.status === 200 });

    return { token: res.json("accessToken") };
}

/**
 * @returns {Promise<string>}
 */
async function shortenUrl({ token }) {
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

    check(res, {
        "test URL was shortened": (r) => r.status === 201,
    });

    return res.json("shortUrl");
}

/**
 * @returns {Promise<{shortUrl: string}>}
 */
export async function setup() {
    const { token } = await login();
    const shortUrl = await shortenUrl({ token });

    return { shortUrl };
}

/**
 * @param shortUrl {string}
 */
export default function ({ shortUrl }) {
    const res = http.get(shortUrl, { redirects: 0 });

    check(res, {
        "short URL resolved and got 302": (res) => res.status === 302,
    });

    sleep(1);
}
