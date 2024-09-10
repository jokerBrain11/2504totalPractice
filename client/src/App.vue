<template>
  <div id="app">
    <layout :isAuthenticated="isAuthenticated">
      <router-view />
    </layout>
  </div>
</template>

<script>
import { mapGetters } from 'vuex';
import layout from '@/components/layout.vue';

export default {
  name: 'App',
  components: {
    layout,
  },
  computed: {
    ...mapGetters('user', ['isAuthenticated']),
  },

};
</script>


