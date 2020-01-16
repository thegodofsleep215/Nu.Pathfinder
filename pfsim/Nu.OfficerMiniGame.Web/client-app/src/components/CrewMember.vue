<template>
    <div>
        <div class="card">
            <div class="container">
                <span>{{crewMember.name}}</span> - <span>{{crewMember.title}}</span>
            </div>
        </div>
    </div>
</template>

<script>
    export default {
        name: "CrewMember",
        props: {
            crewMember: {}
        }
    }
</script>

<style>
</style>