<template>
    <div>
        <div class="card">
            <div class="container">
                <span>{{crewMember.name}}</span><span v-if="crewMember.isAssistant"> - Assistant</span>
                <button style="float: right; margin-left: 10px" class="small-delete-button" v-on:click="raiseDelete"></button>
            </div>
        </div>
    </div>
</template>

<script>
    export default {
        name: "CrewMemberLoadout",
        props: {
            crewMember: {}
        },
        methods: {
            raiseDelete() {
                this.$emit("delete_crew", this.crewMember);
            }
        }
    }
</script>

<style>
</style>