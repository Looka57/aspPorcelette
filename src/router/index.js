import { createRouter, createWebHistory } from 'vue-router';
// Importe tes vues réelles ici
// import HomeView from '../views/HomeView.vue';
// import AdminDashboard from '../views/AdminDashboard.vue';

const routes = [
  {
    path: '/',
    name: 'home',
    component: () => import('../views/HomeView.vue') // Adapte le chemin
  },
  {
    path: '/admin',
    name: 'admin',
    component: () => import('../views/AdminDashboard.vue'), // Ta page avec la liste des Senseis
    meta: { requiresAuth: true } // On marque cette route comme "protégée"
  },
  {
    path: '/login',
    name: 'login',
    component: () => import('../views/LoginView.vue') // Tu devras créer cette petite vue
  }
];

const router = createRouter({
  history: createWebHistory(),
  routes
});

// LE GARDE : Il s'exécute à chaque changement d'URL
router.beforeEach((to, from, next) => {
  // On cherche si un jeton "user-auth" existe dans le navigateur
  const isAuthenticated = localStorage.getItem('user-auth');

  if (to.meta.requiresAuth && !isAuthenticated) {
    // Si la page est privée et qu'on n'est pas connecté -> redirection login
    next('/login');
  } else {
    // Sinon (page publique ou déjà connecté) -> on laisse passer
    next();
  }
});

export default router;