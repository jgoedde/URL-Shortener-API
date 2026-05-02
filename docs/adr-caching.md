# ADR-002: Caching Strategy for Fast Redirects

## Status

Accepted, implemented

## Context

The redirect endpoint is the hot path of the application. Every click on a shortened URL hits this endpoint, meaning it
will receive significantly more traffic than the write (shorten) endpoint. Without caching, every redirect results in a
database query, which adds latency and puts unnecessary load on PostgreSQL under high concurrency.

## Decision

Use Redis as a distributed cache with a cache-aside pattern implemented as a MediatR pipeline behaviour (
`QueryCachingPipelineBehaviour`). Any query implementing `ICachedQuery` is automatically cached without modifying the
handler itself. Short code → original URL mappings are cached with a 24 hour TTL.

In-memory caching was considered but rejected because it does not scale horizontally - each instance would maintain its
own warm-up cycle and cache state would be inconsistent across instances.

## Consequences

- Redirect p(99) dropped from 77ms to 8.6ms under load (200 VUs)
- Adding caching to any future query requires only implementing `ICachedQuery` — no changes to the handler
- Redis is now a required infrastructure dependency
- URL mappings are immutable so cache invalidation is a non-issue for now — if URL blocking is added later, explicit
  cache key deletion will be required on block
