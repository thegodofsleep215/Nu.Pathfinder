<template>
    <div class="card">
        <div class="container">
            <div class="form-grid-container">

                <div>
                    <h3>{{data.name}}</h3>
                </div>
                <div>
                    <button style="align-self:center" class="update-button" v-on:click="saveCrewMember(data)">
                    </button>
                </div>

                <div>
                    <table align="center">
                        <tr>
                            <td style="text-align:right">
                                <label class="stat-label">Craft (Carpentry): </label>
                            </td>
                            <td>
                                <div><input type="number" style="float:left" v-model="data.skills.craftCarpentry" /></div>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align:right">
                                <div><label class="stat-label">Craft (Cooking): </label></div>
                            </td>
                            <td>
                                <div><input type="number" v-model="data.skills.craftCooking" /></div>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align:right">
                                <div><label class="stat-label">Craft (Sails): </label></div>
                            </td>
                            <td>
                                <div><input type="number" v-model="data.skills.craftSails" /></div>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align:right">
                                <div><label class="stat-label">Craft (Ship): </label></div>
                            </td>
                            <td>
                                <div><input type="number" v-model="data.skills.craftShip" /></div>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align:right">
                                <div><label class="stat-label">Diplomacy: </label></div>
                            </td>
                            <td>
                                <div><input type="number" v-model="data.skills.diplomacy" /></div>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align:right">
                                <div><label class="stat-label">Heal: </label></div>
                            </td>
                            <td>
                                <div><input type="number" v-model="data.skills.heal" /></div>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align:right">
                                <div><label class="stat-label">Intimidate: </label></div>
                            </td>
                            <td>
                                <div><input type="number" v-model="data.skills.intimidate" /></div>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align:right">
                                <div><label class="stat-label">Knowledge (Engineering): </label></div>
                            </td>
                            <td>
                                <div><input type="number" v-model="data.skills.knowledgeEngineering" /></div>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align:right">
                                <div><label class="stat-label">Perform: </label></div>
                            </td>
                            <td>
                                <div><input type="number" v-model="data.skills.perform" /></div>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align:right">
                                <div><label class="stat-label">Profession (Merchant): </label></div>
                            </td>
                            <td>
                                <div><input type="number" v-model="data.skills.professionMerchant" /></div>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align:right">
                                <div><label class="stat-label">Profession (Sailor): </label></div>
                            </td>
                            <td>
                                <div><input type="number" v-model="data.skills.professionSailor" /></div>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align:right">
                                <div><label class="stat-label">Survival: </label></div>
                            </td>
                            <td>
                                <div><input type="number" v-model="data.skills.survival" /></div>
                            </td>
                        </tr>
                    </table>

                </div>
            </div>
        </div>
    </div>
</template>

<script>
    export default {
        name: "CrewDetail",
        props: {
            data: {}
        },
        methods: {
            saveCrewMember(crewMember) {
                fetch('/Crew/Update', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify(crewMember)
                });
            }
        }
    }
</script>

<style>
    .form-grid-container {
        display: grid;
        grid-template-columns: auto 20px;
        grid-column-gap: 2px;
        grid-row-gap: 5px;
        padding: 5px;
        justify-items: center;
        align-items: center;
    }
</style>