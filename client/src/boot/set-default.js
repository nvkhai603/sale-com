// Folowing https://github.com/quasarframework/quasar/discussions/8761#discussioncomment-1042529
const setDefault = (component, key, value) => {
    const prop = component.props[key];
    switch (typeof prop) {
      case "object":
        prop.default = value;
        break;
      case "function":
        component.props[key] = {
          type: prop,
          default: value
        };
        break;
      case "undefined":
        throw new Error("unknown prop: " + key);
        break;
      default:
        throw new Error("unhandled type: " + typeof prop);
        break;
    }
  };
  import { boot } from 'quasar/wrappers'
  import { QInput } from "quasar";
  import { QBtn } from 'quasar';
  export default boot(({ app }) => {
    setDefault(QInput, "outlined", true);
    setDefault(QInput, "dense", true);
    setDefault(QInput, "stackLabel", false);
    setDefault(QInput, "hideBottomSpace", true);
    setDefault(QBtn, "noCaps", true);
  })
