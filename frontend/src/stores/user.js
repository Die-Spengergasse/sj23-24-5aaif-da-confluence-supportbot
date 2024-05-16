import { ref } from 'vue'
import { defineStore } from 'pinia'

export const useUserStore = defineStore('user', () => {
  const userdata = ref(0);
  function setUserdata(value) {
    userdata.value = value;
  }

  return { userdata, setUserdata }
});