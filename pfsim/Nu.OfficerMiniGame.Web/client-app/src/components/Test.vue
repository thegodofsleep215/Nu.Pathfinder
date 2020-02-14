<template>
    <div>
        <span>{{one.msg}} = {{two.msg}}</span>
    </div>
</template>

<script>
    export default {
        name: "Test",
        props: {
            one: {},
            two: {}
        }
    }
</script>