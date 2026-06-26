import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth.guard';
import { librarianGuard } from './core/guards/librarian.guard';

export const routes: Routes = [
  {
    path: 'login',
    loadComponent: () => import('./features/auth/login/login').then(m => m.Login)
  },
  {
    path: 'register',
    loadComponent: () => import('./features/auth/register/register').then(m => m.Register)
  },
  {
    path: '',
    loadComponent: () => import('./features/layout/shell/shell').then(m => m.Shell),
    canActivate: [authGuard],
    children: [
      { path: '', pathMatch: 'full', redirectTo: 'books' },

      // Books
      { path: 'books', loadComponent: () => import('./features/books/book-list/book-list').then(m => m.BookList) },
      { path: 'books/new', canActivate: [librarianGuard], loadComponent: () => import('./features/books/book-form/book-form').then(m => m.BookForm) },
      { path: 'books/:id', loadComponent: () => import('./features/books/book-detail/book-detail').then(m => m.BookDetail) },
      { path: 'books/:id/edit', canActivate: [librarianGuard], loadComponent: () => import('./features/books/book-form/book-form').then(m => m.BookForm) },

      // Authors
      { path: 'authors', loadComponent: () => import('./features/authors/author-list/author-list').then(m => m.AuthorList) },
      { path: 'authors/new', canActivate: [librarianGuard], loadComponent: () => import('./features/authors/author-form/author-form').then(m => m.AuthorForm) },
      { path: 'authors/:id/edit', canActivate: [librarianGuard], loadComponent: () => import('./features/authors/author-form/author-form').then(m => m.AuthorForm) },

      // Borrows
      { path: 'my-borrows', loadComponent: () => import('./features/borrows/my-borrows/my-borrows').then(m => m.MyBorrows) },
      { path: 'borrows', canActivate: [librarianGuard], loadComponent: () => import('./features/borrows/all-borrows/all-borrows').then(m => m.AllBorrows) },

      // Members
      { path: 'profile', loadComponent: () => import('./features/members/profile/profile').then(m => m.Profile) },
      { path: 'members', canActivate: [librarianGuard], loadComponent: () => import('./features/members/member-list/member-list').then(m => m.MemberList) },
      { path: 'members/:id', canActivate: [librarianGuard], loadComponent: () => import('./features/members/member-detail/member-detail').then(m => m.MemberDetail) }
    ]
  },

  { path: '**', redirectTo: '' }
];
