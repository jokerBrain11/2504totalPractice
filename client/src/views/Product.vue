<template>
  <div class="container mt-5">
    <h1 class="text-center mb-4">產品列表</h1>
    <div class="row">
      <div v-for="product in products" :key="product.id" class="col-md-4 mb-4">
        <product-card :product="product" @add-to-cart="addToCart"></product-card>
      </div>
    </div>
  </div>
</template>

<script>
import { mapState, mapActions } from "vuex";
import ProductCard from "../components/ProductCard.vue";

export default {
  components: {
    ProductCard,
  },
  computed: {
    ...mapState("product", ["products"]),
  },
  methods: {
    ...mapActions("product", ["addToCart", "fetchProducts"]),
  },
  created() {
    this.fetchProducts();
  },
};
</script>

<style scoped>
.container {
  max-width: 1200px;
}

h1 {
  font-size: 2.5rem;
  font-weight: bold;
}

.row {
  justify-content: center;
}

.product-card {
  border: 1px solid #ddd;
  border-radius: 5px;
  padding: 15px;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
}

.product-card:hover {
  box-shadow: 0 6px 12px rgba(0, 0, 0, 0.15);
}
</style>
