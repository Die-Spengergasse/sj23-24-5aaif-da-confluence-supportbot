<script setup>
import { useUserStore } from "@/stores/user";
import { RouterLink, RouterView } from "vue-router";

const user = useUserStore();
</script>

<template>
  <div class="patter">
    <header>
      <div class="flex-container">
        <!-- <div class="item1">
          <nav class="leftside">
            <a>Terms of Service</a>
            <a>Help</a>
          </nav>
        </div> -->

        <div class="item2">
          <a>Confluence SupportBot</a>
          <nav class="Profil">
            <img src="@/assets/profil_icon.png" />
            <h5 class="navbarHeader">
              Angemeldet als {{ user.userdata.username }}
              <!-- <small>({{ user.userdata.guid }})</small> -->
            </h5>
          </nav>
        </div>

        <!-- <div class="item3">
          <nav class="rightside">
            <a>Team</a>
            <a>Impressum</a>
          </nav>
        </div> -->
      </div>
    </header>

    <div class="einfuehrung">
      <p>
        Nutze den SupportBot, um Dokumentationen schneller und effizienter zu
        finden! <br />Er erleichtert die Suche erheblich und spart dir wertvolle
        Zeit.
      </p>
    </div>

    <div class="bot_icon">
      <img src="@/assets/bot_icon.png" />
    </div>
  </div>
  <main>
    <RouterView />
  </main>
</template>

<style scoped>
header {
  background-color: #007bff;
  color: #fff;
  padding: 10px 20px;
  display: flex;
  justify-content: center;
}

nav {
  display: flex;
  gap: 20px;
  justify-content: center;
}

nav a {
  color: #fff;
  text-decoration: none;
  text-align: center;
  align-items: center;
  padding-right: 40px;
}

.flex-container {
  display: flex;
  justify-content: center;
  align-items: flex-start;
}

.flex-container > div {
  border-radius: 5px;
  padding: 8px;
  padding-right: 40px;
}

.item1 {
  flex-grow: 2;
  align-items: center;
  text-align: center;
  font-family: "Trebuchet MS";
}

.item2 {
  flex-grow: 1;
  font-size: 30px;
  font-family: "Trebuchet MS";
  font-weight: 90000;
}

.item3 {
  flex-grow: 2;
  align-self: auto;
  font-family: "Trebuchet MS";
}

.Profil {
  block-size: 30px;
  justify-items: flex-end;
  display: flex;
  align-items: center;
  gap: 10px;
  font-size: 20px;
}

.Profil img {
  block-size: 30px;
}

.einfuehrung {
  font-family: "Trebuchet MS";
  font-weight: 800;
  text-align: center;
  padding-block: 40px;
}

.bot_icon {
  block-size: 150px;
  background-color: #007bff;
  display: flex;
  justify-content: center;
  align-items: end;

  border-top-left-radius: 80px;
  border-top-right-radius: 80px;
}

.bot_icon img {
  height: 80%;
  border: 5px solid #000;
  background-color: #fff;
  border-radius: 50%;
}
</style>
