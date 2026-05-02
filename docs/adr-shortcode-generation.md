# ADR-001: Shortcode Generation Strategy

## Status

Accepted, implemented

## Context

A URL shortener requires a mechanism to generate short, unique identifiers (shortcodes) that map to original URLs. The
shortcode is the public-facing part of the short URL and directly impacts user experience, system performance, and
security posture.

Key requirements:

- Must be URL-safe (no special characters)
- Must be unique across all entries
- Should be compact and reasonably human-readable
- Should perform well at scale

Three approaches were evaluated.

### Option 1: UUID v4

Generates a random 128-bit identifier (e.g. `550e8400-e29b-41d4-a716-446655440000`).

**Pros:**

- Globally unique without coordination
- Not guessable or enumerable

**Cons:**

- Not URL-friendly in raw form (contains hyphens)
- Long (36 characters), poor UX
- Worse database index performance due to random insertion order

### Option 2: Raw Database Incrementing ID

Uses the database's native auto-increment primary key directly as the shortcode (e.g. `1`, `2`, `4201`).

**Pros:**

- Simple to implement, no encoding step
- Sequential, optimal for B-tree index performance
- No collision risk

**Cons:**

- Numeric only, not compact at large values
- Fully guessable and enumerable; exposes business metrics (e.g. total link count)

### Option 3: Base62 Encoding of Incrementing ID ✓

Uses the database's auto-increment primary key internally, but encodes it in Base62 (`[A-Za-z0-9]`) for the public
shortcode (e.g. ID `12345` → shortcode `dnh`).

**Pros:**

- URL-safe alphabet, no special characters needed
- Compact — Base62 is significantly shorter than raw integers at large values
- Retains database performance benefits of sequential primary keys
- Deterministic and reversible; easy to decode for debugging

**Cons:**

- Shortcodes are technically reversible; given a shortcode, the underlying ID can be derived
- Sequential in structure — an observer can infer approximate volume over time, though not exact counts without gaps

## Decision

We will use **Base62 encoding of the auto-incrementing internal primary key** as the shortcode generation strategy.

The internal primary key remains a standard integer auto-increment column. When a new URL is stored, the assigned
primary key is encoded to Base62 and used as the public shortcode. The shortcode is stored alongside the URL for fast
lookup.

The Base62 alphabet used is: `0-9A-Za-z` (digits first, then uppercase, then lowercase).

Using [SimpleBase](https://github.com/ssg/SimpleBase) library for Base62 encoding.

## Consequences

**Easier:**

- Shortcode uniqueness is guaranteed by the database's primary key constraint; no collision handling needed
- Database queries remain performant due to sequential key insertion
- Shortcodes are human-typeable and URL-safe without any encoding
- Debugging is straightforward — shortcodes can be decoded back to their primary key

**Harder / Trade-offs:**

- The sequential nature of IDs means shortcodes are loosely enumerable; this is an accepted risk as the service does not
  require link privacy by default
- Changing the encoding scheme later would require a migration of all existing shortcodes or a dual-lookup strategy
