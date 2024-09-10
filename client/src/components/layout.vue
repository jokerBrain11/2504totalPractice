<!-- Layout.vue -->
<template>
    <header class="bg-dark text-white p-3">
        <div class="container-fluid">
            <div class="d-flex justify-content-between align-items-center">
                <img class="logo" src="@/assets/logo.png" width="100" height="100" />
                <nav class="nav">
                    <RouterLink v-show="!isAuthenticated" class="nav-link text-white" to="/login">Login</RouterLink>
                    <RouterLink v-show="!isAuthenticated" class="nav-link text-white" to="/register">Register
                    </RouterLink>
                    <RouterLink v-show="isAuthenticated" class="nav-link text-white" to="/product">Product</RouterLink>
                    <RouterLink v-show="isAuthenticated" class="nav-link text-white" to="/cart">Cart</RouterLink>
                    <RouterLink v-show="isAuthenticated" class="nav-link text-white" to="/user-profile">User Profile
                    </RouterLink>
                    <button v-show="isAuthenticated" @click="$logout" class="nav-link text-white">Logout</button>
                </nav>
            </div>
        </div>
    </header>
    <main class="container mt-4">
        <RouterView />
    </main>
</template>

<script>
export default {
    name: 'Layout',
    props: {
        isAuthenticated: {
            type: Boolean,
            default: false
        }
    },
};

</script>

<style scoped>
header {
  line-height: 1.5;
}

.logo {
  display: block;
}

nav .nav-link {
  padding: 0 1rem;
  font-size: 16px;
}

nav .nav-link:hover {
  text-decoration: underline;
}

.container {
  max-width: 1200px;
}

@media (min-width: 1024px) {
  header {
    padding-right: 2rem;
  }
}
</style>