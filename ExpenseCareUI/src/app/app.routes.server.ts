import { RenderMode, ServerRoute } from '@angular/ssr';

export const serverRoutes: ServerRoute[] = [
  {
    path: 'login',
    renderMode: RenderMode.Client    // ✅ login runs on browser only
  },
  {
    path: '**',
    renderMode: RenderMode.Prerender // everything else stays SSR
  }
];