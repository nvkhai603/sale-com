<template>
  <div class="fullscreen q-pa-sm">
    <!-- BUTTON -->
    <div>
      <h5>Button</h5>
      <div class="q-gutter-sm">
            <q-btn color="white" text-color="black" label="Standard" />
            <q-btn color="primary" label="Primary" />
            <q-btn color="secondary" label="Secondary" />
            <q-btn color="amber" glossy label="Amber" />
            <q-btn color="brown-5" label="Brown 5" />
            <q-btn color="deep-orange" glossy label="Deep Orange" />
            <q-btn color="purple" label="Purple" />
            <q-btn color="black" label="Black" />
      </div>
    </div>
    <!-- FORM -->
    <div>
      <h5>Form</h5>
      <q-input/>
    </div>
    <div>
      <h5>CheckBox</h5>
      <q-checkbox label="Có thể bán hàng âm." />
    </div>
  </div>
</template>

<script>
import { defineComponent } from "vue";

export default defineComponent({
  name: "DesignSystem",
});
</script>
