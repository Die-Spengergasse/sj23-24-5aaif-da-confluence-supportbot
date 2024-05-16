<script setup>
import axios from 'axios';
</script>

<template>
  <div class="home">
    <h1>HomeView</h1>
    <textarea v-model="query" placeholder="Suchanfrage"></textarea>
    <button v-on:click="sendQuery()">Senden</button>
    <template v-if="results.length">
      <h2>Ergebnisse</h2>
      <ul>
        <li v-for="result in results" v-bind:key="result.title">
          {{ result }}
        </li>
      </ul>
    </template>
  </div>
</template>

<script>
export default {
  data() {
    return {
      query: "",
      results: []
    };
  },
  async mounted() {

  },
  methods: {
    async sendQuery() {
      const response = await axios.get("search", { params: { query: this.query } });
      this.results = response.data;
    }
  }
};
</script>

<style scoped>
.error {
  color: red;
  font-size: 80%;
}
</style>
