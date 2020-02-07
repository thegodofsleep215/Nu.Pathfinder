<template>
    <div class="modal-backdrop" v-show="value">
        <div class="modal">
            <section class="modal-body">
                <slot name="body">
                    <label class="stat-label">Name: </label><input v-model="name" />
                </slot>
            </section>
            <footer class="modal-footer">
                <slot name="footer">
                    <button type="button" @click="onOk(name)">Ok</button>
                    <button type="button" @click="onCancel">Cancel</button>
                </slot>
            </footer>
        </div>
    </div>
</template>

<script>
    export default {
        name: "AddModal",
        props: {
            value: {required:false}
        },
        data: function () {
            return { name: "" }
        },
        methods: {
            onOk(name) {
                this.$emit("modal-ok", name);
            },
            onCancel() {
                this.$emit("modal-cancel");
            }
        },
    }
</script>
