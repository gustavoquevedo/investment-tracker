---
description: Frontend development standards, best practices, and conventions for React/TypeScript applications.
globs: ["**/frontend/**/*.ts", "**/frontend/**/*.tsx", "**/frontend/**/*.css", "**/frontend/package.json", "**/*.tsx", "**/*.jsx"]
alwaysApply: true
---

# Frontend Project Standards and Best Practices

## Technology Stack

- **Framework**: React (Latest stable)
- **Build Tool**: Vite
- **Language**: TypeScript
- **Styling**: Tailwind CSS (preferred) or CSS Modules
- **State Management**: React Context API (for simple state) or Zustand (for complex global state)
- **Data Fetching**: React Query (TanStack Query) or SWR
- **Routing**: React Router
- **Testing**: Vitest, React Testing Library
- **Linting/Formatting**: ESLint, Prettier

## Project Structure

Recommended directory structure:

```text
frontend/
  src/
    api/            # API client and endpoints definitions
    assets/         # Static assets (images, fonts)
    components/     # Shared/Common UI components (Button, Input, Card)
    features/       # Feature-based modules
      FeatureName/
        components/ # Components specific to this feature
        hooks/      # Hooks specific to this feature
        types.ts    # Types specific to this feature
    hooks/          # Shared custom hooks
    layouts/        # Page layouts (MainLayout, AuthLayout)
    pages/          # Page components (routed)
    stores/         # Global state stores (if using Zustand)
    types/          # Shared TypeScript interfaces
    utils/          # Helper functions
    App.tsx         # Root component
    main.tsx        # Entry point
```

## Coding Standards

### Component Architecture

- **Functional Components**: Use React Functional Components with Hooks
- **PascalCase**: Component filenames and function names must be PascalCase (e.g., `AssetCard.tsx`)
- **Props Interface**: Define a TypeScript interface for props named `[ComponentName]Props`
- **Composition**: Prefer composition over inheritance. Break down complex components into smaller, reusable pieces

### State Management

- **Local State**: Use `useState` for UI state local to a component
- **Server State**: Use React Query/SWR to manage data from the API (caching, loading, error states)
- **Global State**: Use Context or Zustand only for state shared across many disconnected components (e.g., User Session, Theme). Avoid Redux unless absolutely necessary

### Styling

- **Tailwind CSS**: Use utility classes for styling
- **CSS Variables**: Use CSS variables for theme colors to support dark mode easily
- **Responsive Design**: Mobile-first approach

## Testing Standards

- **Unit Tests**: Test utility functions and hooks
- **Component Tests**: Use React Testing Library to test components. Focus on user interaction and accessibility (e.g., "User clicks button", not "State is updated")
- **Mocking**: Mock API calls using MSW (Mock Service Worker) or simple jest mocks

## Performance & Accessibility

- **Lazy Loading**: Use `React.lazy` and `Suspense` for route-based code splitting
- **Memoization**: Use `useMemo` and `useCallback` judiciously to prevent expensive re-renders (but don't premature optimize)
- **Accessibility (a11y)**:
  - Use semantic HTML tags (`<nav>`, `<main>`, `<article>`, `<button>`)
  - Ensure sufficient color contrast
  - Provide `alt` text for images
  - Support keyboard navigation
