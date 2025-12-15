import { createRouter, createWebHistory } from "vue-router";
import { useAuthStore } from "@/stores/auth";
import FrontLayout from "@/FrontLayout.vue";
import NotFoundView from "@/views/NotFoundView.vue"; 

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  scrollBehavior() {
    return { top: 0 };
  },
  routes: [
    // --- ROUTES PUBLIQUES (Avec menu FrontLayout) ---
    {
      path: "/",
      component: FrontLayout,
      children: [
        { path: "", name: "home", component: () => import("@/views/HomeView.vue") },
        { path: "judo", name: "judo", component: () => import("@/views/JudoView.vue") },
        { path: "aikido", name: "aikido", component: () => import("@/views/AikidoView.vue") },
        { path: "jujitsu", name: "jujitsu", component: () => import("@/views/JujitsuView.vue") },
        { path: "judo-detente", name: "judo-detente", component: () => import("@/views/JudoDetenteView.vue") },
        { path: "equipe", name: "sensei", component: () => import("@/views/SenseiSiteView.vue") },
        { path: "equipeDetailView/:id", name: "senseiDetailView", component: () => import("@/views/SenseiDetailView.vue"), props: true },
        { path: "actualites", name: "actualites", component: () => import("@/views/ActualiteSiteView.vue") },
        { path: "actualite/:id", name: "ActualiteDetail", component: () => import("@/views/ActualiteDetail.vue"), props: true },
        { path: "evenements", name: "evenements", component: () => import("@/views/EvenementsView.vue") },
        { path: "evenement/:id", name: "EvenementDetail", component: () => import("@/views/EvenementDetail.vue"), props: true },
        { path: "tarifs", name: "tarifs", component: () => import("@/views/TarifSiteView.vue") },
      ],
    },

    // --- ROUTES SANS LAYOUT (Celles-ci iront dans le bloc "else" de App.vue) ---
    {
      path: "/login",
      name: "login",
      component: () => import("@/views/LoginView.vue"),
    },
    {
      path: "/403",
      name: "forbidden",
      component: () => import("@/views/ForbiddenView.vue"),
    },
    {
      path: "/404",
      name: "not-found",
      component: NotFoundView, // IMPORTANT: Ne pas mettre dans children
    },

    // --- ROUTES ADMIN ---
    {
      path: "/admin",
      meta: { requiresAuth: true, roles: ["Admin", "Sensei"] },
      children: [
        { path: "dashboard", name: "admin-dashboard", component: () => import("@/views/AdminDashboard.vue") },
        { path: "sensei", name: "admin-sensei", component: () => import("@/views/SenseiView.vue") },
        { path: "licencies", name: "admin-licencies", component: () => import("@/views/LicenciesView.vue") },
        { path: "cours", name: "admin-cours", component: () => import("@/views/CoursView.vue") },
        { path: "events", name: "admin-events", component: () => import("@/views/EventsView.vue") },
        { path: "actualite", name: "admin-actualite", component: () => import("@/views/ActualiteView.vue") },
        { path: "discipline", name: "admin-discipline", component: () => import("@/views/DisciplineView.vue") },
        { path: "tarifs", name: "admin-tarifs", component: () => import("@/views/TarifsView.vue") },
        { path: "compta", name: "admin-compta", component: () => import("@/views/ComptabiliteView.vue") },
        { path: "comptes/:id", name: "admin-compte-details", component: () => import("@/views/CompteDetails.vue"), props: true },
        { path: "profile", name: "admin-profile", component: () => import("@/views/ProfilePageView.vue") },
      ],
    },

    // --- CATCH-ALL (Redirection directe) ---
    { 
      path: "/:pathMatch(.*)*", 
      redirect: "/404" 
    },
  ],
});


// Navigation Guard
router.beforeEach(async (to, from, next) => {
  const authStore = useAuthStore();

  if (authStore.isLoggedIn && !authStore.user) {
    try {
      await authStore.fetchProfile();
    } catch (e) {
      authStore.logout();
      return next("/login");
    }
  }

  if (to.meta.requiresAuth) {
    if (!authStore.isLoggedIn) return next("/login");

    const requiredRoles = to.meta.roles || [];
    const userRoles = authStore.user?.roles || [];
    const hasRequiredRole = requiredRoles.some((role) => userRoles.includes(role));

    if (!hasRequiredRole) return next("/403");
  }

  next();
});

export default router;