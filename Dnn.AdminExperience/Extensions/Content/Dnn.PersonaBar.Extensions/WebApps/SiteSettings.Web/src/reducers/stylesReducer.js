import { styles as ActionTypes} from "../constants/actionTypes";

export default function styles(state = {}, action) {
    switch (action.type) {
        case ActionTypes.CHANGED_PORTAL_ID:
            return { ...state,
                portalId: action.data.portalId
            };
        case ActionTypes.RETRIEDVED_STYLES_SETTINGS:
            return { ...state,
                styles: action.data.styles,
                clientModified: action.data.clientModified
            };
        case ActionTypes.STYLES_SETTINGS_CLIENT_MODIFIED:
            return { ...state,
                styles: action.data.styles,
                clientModified: action.data.clientModified
            };
        case ActionTypes.UPDATED_STYLES_SETTINGS:
            return { ...state,
                clientModified: action.data.clientModified
            };
        default:
            return state;
    }
}