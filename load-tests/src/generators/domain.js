import { faker } from "@faker-js/faker/locale/de";

export function generateUrlToShorten() {
    return faker.internet.url();
}
