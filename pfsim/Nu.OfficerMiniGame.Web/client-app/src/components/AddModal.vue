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
                    <button type="button" @click="OnCancel()">Cancel</button>
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
                this.$emit("add-item", name);
            },
            onCancel() {
                this.$emit("add-cancel");
            }
        },
    }
</script>

<style>
    .modal-backdrop {
        position: fixed;
        top: 0;
        bottom: 0;
        left: 0;
        right: 0;
        background-color: rgba(0, 0, 0, 0.3);
        display: flex;
        justify-content: center;
        align-items: center;
    }

    .modal {
        background: #FFFFFF;
        box-shadow: 2px 2px 20px 1px;
        overflow-x: auto;
        display: flex;
        flex-direction: column;
    }

    .modal-header,
    .modal-footer {
        padding: 15px;
        display: flex;
    }

    .modal-header {
        border-bottom: 1px solid #eeeeee;
        color: #4AAE9B;
        justify-content: space-between;
    }

    .modal-footer {
        border-top: 1px solid #eeeeee;
        justify-content: flex-end;
    }

    .modal-body {
        position: relative;
        padding: 20px 10px;
    }
</style>