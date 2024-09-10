<template>
  <div class="container mt-5">
    <div class="card">
      <div class="card-body">
        <form @submit.prevent="updateUserProfile">
          <h1 class="card-title text-center">用戶個人信息</h1>
          <hr />
          <div class="input-group mb-3">
            <span class="input-group-text" id="basic-addon3">使用者帳號:</span>
            <input type="text" class="form-control" id="basic-url" v-model="form.username"
              aria-describedby="basic-addon3">
          </div>
          <div class="input-group mb-3">
            <span class="input-group-text" id="basic-addon3">電子郵件:</span>
            <input type="text" class="form-control" id="basic-url" v-model="form.email" aria-describedby="basic-addon3">
          </div>
          <div class="input-group mb-3">
            <span class="input-group-text" id="basic-addon3">姓:</span>
            <input type="text" class="form-control" id="basic-url" v-model="form.firstName"
              aria-describedby="basic-addon3">
          </div>
          <div class="input-group mb-3">
            <span class="input-group-text" id="basic-addon3">名:</span>
            <input type="text" class="form-control" id="basic-url" v-model="form.lastName"
              aria-describedby="basic-addon3">
          </div>
          <div class="input-group mb-3">
            <span class="input-group-text" id="basic-addon3">手機:</span>
            <input type="text" class="form-control" id="basic-url" v-model="form.phone" aria-describedby="basic-addon3">
          </div>
          <div class="input-group mb-3">
            <span class="input-group-text" id="basic-addon3">地址:</span>
            <input type="text" class="form-control" id="basic-url" v-model="form.address"
              aria-describedby="basic-addon3">
          </div>
          <button type="submit" class="btn btn-primary">更新</button>
        </form>
      </div>
    </div>
  </div>
</template>

<script>
import api from '@/services/api';

const initForm = {
  username: '',
  email: '',
  firstName: '',
  lastName: '',
  phone: '',
  address: ''
};

export default {
  data() {
    return {
      form: { ...initForm }
    };
  },
  mounted() {
    this.getUserProfile();
  },
  methods: {
    async getUserProfile() {
      try {
        const res = await api.userProfile();
        if (res.code === 200) {
          this.form = { ...res.data };
        }
        else {
          this.$router.push({ name: 'Login' });
        }
      }
      catch (error) {
        console.error(error);
        this.$router.push({ name: 'Login' });
      }
    },
    async updateUserProfile(){
      try
      {
        const res = await api.updateUserProfile(this.form);
        if (res.code === 200) {
          this.getUserProfile();
        }
      }
      catch (error) {
        console.error(error);
      }
    }
  },
};
</script>

<style scoped>
.container {
  max-width: 600px;
}

.card {
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
  border-radius: 8px;
}

.card-title {
  font-size: 1.8rem;
  font-weight: bold;
}

.card-text {
  font-size: 1.2rem;
}

.btn-danger {
  background-color: #dc3545;
  border-color: #dc3545;
}

.btn-danger:hover {
  background-color: #c82333;
  border-color: #bd2130;
}
</style>
