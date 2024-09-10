<template>
  <div id="count">
    <h1>計數器應用</h1>
    <p>當前計數: {{ count }}</p>
    <p>雙倍計數: {{ doubleCount }}</p>
    <button @click="increment">增量</button>
    <button @click="decrement">減量</button>
  </div>
</template>

<script>
export default {
  computed: {
    count() {
      return this.$store.state.count;
    },
    doubleCount() {
      return this.$store.getters.doubleCount;
    }
  },
  methods: {
    increment() {
      this.$store.dispatch('increment');
    },
    decrement() {
      this.$store.dispatch('decrement');
    }
  }
};
</script>