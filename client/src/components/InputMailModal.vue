<template>
  <div
    class="modal fade"
    :id="modalName"
    tabindex="-1"
    :aria-labelledby="modalName"
    aria-hidden="true"
  >
    <div class="modal-dialog">
      <div class="modal-content">
        <div class="modal-header">
          <h5 class="modal-title" :id="modalName">
            {{ this.title }}
          </h5>
          <button
            type="button"
            class="btn-close"
            data-bs-dismiss="modal"
            aria-label="Close"
            @click="clearData"
          ></button>
        </div>
        <div class="modal-body">
          <form @submit.prevent="submitForm">
            <div class="mb-3">
              <label for="email" class="form-label">請輸入您的 Email</label>
              <input
                type="email"
                v-model="email"
                class="form-control"
                placeholder="請輸入 Email"
                required
              />
            </div>
            <div class="modal-footer">
              <button
                type="button"
                class="btn btn-secondary"
                data-bs-dismiss="modal"
                @click="clearData"
              >
                關閉
              </button>
              <button type="submit" class="btn btn-primary">
                {{ this.title }}
              </button>
            </div>
          </form>
        </div>
      </div>
      <p v-if="error" class="text-danger mt-3 text-center">{{ error }}</p>
    </div>
  </div>
</template>

<script>
export default {
  name: "InputMailModal",
  data() {
    return {
      error: "",
      email: "",
    };
  },
  props: {
    title: String,
    modalName: String,
  },
  methods: {
    submitForm() {
      console.log(this.email);
      if (this.email === "") {
        this.error = "請輸入您的 Email";
        return;
      }
      this.$emit("submit-form", this.email);
      this.clearData();
    },
    clearData() {
      this.error = "";
      this.email = "";
    },
  },
};
</script>
