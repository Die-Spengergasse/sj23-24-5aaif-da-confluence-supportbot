import 'primevue/resources/themes/lara-light-green/theme.css'
import './assets/main.css'

import { createApp } from 'vue'
import { createPinia } from 'pinia'
import { useUserStore } from '@/stores/user'
import PrimeVue from 'primevue/config';
import axios from "axios";

/* import the fontawesome core */
import { library } from '@fortawesome/fontawesome-svg-core'
/* import specific icons */
import {  faUsers, faHouse } from '@fortawesome/free-solid-svg-icons'
/* import font awesome icon component */
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome'

import App from './App.vue'
import router from './router'

axios.defaults.baseURL =
    process.env.NODE_ENV == "production" ? "/api" : "https://localhost:5001/api";
axios.defaults.withCredentials = true;

axios
    .get("users/userinfo")
    .then((response) => {
        if (!response.headers["content-type"].includes("application/json"))
            throw "Not authenticated";
        library.add(faUsers, faHouse)

        const app = createApp(App)
        .component('font-awesome-icon', FontAwesomeIcon)
        app.use(PrimeVue);
        app.use(router)
        app.use(createPinia())
        const user = useUserStore();
        user.setUserdata(response.data);
        app.mount('#app')
    })
    .catch(err => {
        let loginUrl = process.env.NODE_ENV == "production" ? "/login" : "https://localhost:5001/login";
        loginUrl += `?returnUrl=${encodeURIComponent(window.location.href)}`;
        window.location.href = loginUrl;
    });
