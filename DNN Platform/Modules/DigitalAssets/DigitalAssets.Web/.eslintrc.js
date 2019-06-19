module.exports = {
    "parser": "@typescript-eslint/parser",
    "parserOptions": {
        "ecmaFeatures": {
            "jsx": true
        },
        "project": "./tsconfig.json",
        "warnOnUnsupportedTypeScriptVersion": true
    },
    "plugins": ["@typescript-eslint"],
    "extends": [
        "eslint:recommended", 
        "plugin:@typescript-eslint/recommended",
        "plugin:react/recommended"
    ],
    "settings": {
        "react":{
            "version": "16"
        }
    },
    "env": {
        "browser": true
    }
}