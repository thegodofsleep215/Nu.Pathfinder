export function getShipTypes() {
    return [
        {
            shipType: "Barkentine",
            shipSize: "Colossal",
            hullHitPoints: 1360,
            officerSize: 4,
            crewSize: 12,
            shipDc: 10,
            cargoPoints: 290,
            passengers: 200,
            specialFeatures: ["Captain's Cabin", "Crow's Nest", "Ship's Galley"],
            propulsionTypes: [{ propulsionType: "Sails", propulsionHitPoints: 960, shipSpeed: 90 }]
        },
        {
            shipType: "Barque",
            shipSize: "Colossal",
            hullHitPoints: 1750,
            officerSize: 4,
            crewSize: 16,
            shipDc: 11,
            cargoPoints: 510,
            passengers: 240,
            specialFeatures: ["Captain's Cabin", "Crow's Nest", "Ship's Galley"],
            propulsionTypes: [{ propulsionType: "Sails", propulsionHitPoints: 960, shipSpeed: 90 }]
        },
        {
            shipType: "Corvette",
            shipSize: "Colossal",
            hullHitPoints: 1310,
            officerSize: 3,
            crewSize: 16,
            shipDc: 9,
            cargoPoints: 510,
            passengers: 240,
            specialFeatures: ["Captain's Cabin", "Crow's Nest", "Ship's Galley", "Fighting Tops", "Sick Bay"],
            propulsionTypes: [{ propulsionType: "Sails", propulsionHitPoints: 440, shipSpeed: 90 }]
        },
        {
            shipType: "MerchantShip",
            shipSize: "Colossal",
            hullHitPoints: 1090,
            officerSize: 4,
            crewSize: 16,
            shipDc: 10,
            cargoPoints: 129,
            passengers: 180,
            specialFeatures: ["Captain's Cabin", "Crow's Nest", "Ship's Galley"],
            propulsionTypes: [{ propulsionType: "Sails", propulsionHitPoints: 452, shipSpeed: 90 }]
        },

    ];
}
