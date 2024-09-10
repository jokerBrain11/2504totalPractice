<template>
  <div class="card product-card">
    <div class="card-body">
      <h2 class="card-title">{{ product.name }}</h2>
      <p class="card-text">價格: ${{ product.price }}</p>
      <slot name="extra-info"></slot>
      <button class="btn btn-primary w-100 mt-3" @click="addToCart">加入購物車</button>
    </div>
  </div>
</template>

<script>
export default {
  props: ['product'],
  methods: {
    addToCart() {
      this.$emit('add-to-cart', this.product);
    },
  },
};
</script>

<style scoped>
.product-card {
  margin-bottom: 20px;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
  transition: box-shadow 0.3s ease;
}

.product-card:hover {
  box-shadow: 0 8px 16px rgba(0, 0, 0, 0.2);
}

.card-title {
  font-size: 1.5rem;
  font-weight: bold;
}

.card-text {
  font-size: 1.2rem;
}

.btn-primary {
  background-color: #007bff;
  border-color: #007bff;
}

.btn-primary:hover {
  background-color: #0056b3;
  border-color: #0056b3;
}
</style>
