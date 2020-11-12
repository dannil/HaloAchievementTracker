module.exports = {
  parser: "@typescript-eslint/parser", // Specifies the ESLint parser
  parserOptions: {
    ecmaVersion: 2020,
    sourceType: "module"
  },
  "extends": [
    "plugin:@typescript-eslint/recommended"
  ],
  "rules": {
    "indent": ["error", 2],

    "@typescript-eslint/no-unused-vars": "off",
    "@typescript-eslint/no-unused-vars-experimental": "warn"
  }
};
