<template>
    <div>
        <ObjectEditor title="Crew Members"
                      url="CrewMemberStats"
                      detailComponent="CrewDetail.vue"></ObjectEditor>
    </div>

</template>

<script>
    import ObjectEditor from './Legos/ObjectEditor.vue'
    export default {
        name: "CrewList",
        components: {
            ObjectEditor
        }
    }
</script>

<style></style>