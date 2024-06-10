<script setup>
import axios from "axios";
</script>

<template>
  <div class="home">
    <textarea
      class="texteingabe"
      v-model="query"
      placeholder="Netzwerkproblem..."
    ></textarea>
    <button class="senden_button" v-on:click="sendQuery()">Suchen</button>
  </div>

  <div class="ausgabe">
    <template v-if="results.length">
      <h2>Ergebnisse</h2>
      <ul>
        <li v-for="result in results" :key="result.title">
          <h3>{{ result.title }}</h3>
          <h6>{{ result.content }}</h6>
        </li>
      </ul>
    </template>
  </div>
</template>

<script>
import axios from "axios";

export default {
  data() {
    return {
      query: "",
      results: [],
    };
  },
  methods: {
    async sendQuery() {
      const response = await axios.get("search", {
        params: { query: this.query },
      });
      this.results = response.data;
    },
  },
};
</script>

<style scoped>
.ausgabe h2 {
  padding-left: 47px;
}
ul {
  list-style-type: none;
  padding: 0;
  padding-left: 35px;
  padding-right: 35px;
}

li {
  margin-bottom: 10px;
  padding: 10px;
  border-radius: 20px; /* Abgerundete Ecken für die Listenelemente */
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1); /* Leichter Schatten für jedes Element */
  border: 5px solid gray;
}

li h3 {
  color: #007bff; /* Blaue Schrift für Titel */
  font-weight: bold;
}

.home {
  block-size: 85px;
  background-color: #007bff;
  display: flex;
  justify-content: center;
  align-items: center;
  padding-top: 10px;
}
.ausgabe {
  border-bottom-left-radius: 80px;
  border-bottom-right-radius: 80px;
  border: 30px solid #007bff;
  box-sizing: border-box;
}
.texteingabe {
  border-bottom-left-radius: 80px;
  border-top-left-radius: 80px;
  padding: 10px;
  border: 1px solid #ccc;
  outline: none;
  width: 350px;
  box-sizing: border-box;
  resize: none;
  height: 40px;
}

.senden_button {
  border-top-right-radius: 80px;
  border-bottom-right-radius: 80px;
  padding: 0;
  height: 40px;
  border: none;
  background-color: #ffffff;
  color: #000;
  cursor: pointer;
  outline: none;
  display: flex;
  align-items: center;
  justify-content: center;
  width: 100px;
}

.error {
  color: red;
  font-size: 80%;
}
</style>