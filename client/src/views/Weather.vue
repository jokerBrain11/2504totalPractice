<template>
  <div>
    <h1>天氣預報</h1>
    <ul>
      <li v-for="forecast in forecasts" :key="forecast.date">
        日期: {{ forecast.date }} - 溫度: {{ forecast.temperatureC }}°C ({{ forecast.temperatureF }}°F) - 狀態: {{ forecast.summary }}
      </li>
    </ul>
  </div>
</template>

<script>
import api from '@/services/api';

export default {
  data() {
    return {
      forecasts: [],
    };
  },
  created() {
    this.fetchWeatherForecast();
  },
  methods: {
    async fetchWeatherForecast() {
      try {
        const response = await api.getWeatherForecast();
        this.forecasts = response.data;
      } catch (error) {
        console.error('無法獲取天氣數據:', error);
      }
    },
  },
};
</script>
