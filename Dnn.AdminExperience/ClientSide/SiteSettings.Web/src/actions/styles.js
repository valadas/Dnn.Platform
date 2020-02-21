import { styles as ActionTypes } from "../constants/actionTypes";
import ApplicationService from "../services/applicationService";

const stylesActions = {
    updatePortalId(portalId) {
        return {
            type: ActionTypes.CHANGED_PORTAL_ID,
            data: {
                portalId
            }
        };
    },
    getPortalStyles(portalId, callback) {
        return (dispatch) => {
            ApplicationService.getStylesSettings(portalId, data => {
                dispatch({
                    type: ActionTypes.RETRIEDVED_STYLES_SETTINGS,
                    data: {
                        styles: data.Settings.Styles,
                        clientModified: false
                    }
                });
                if (callback) {
                    callback(data);
                }
            });
        };
    },
    updatePortalStyles(payload, callback, failureCallback) {
        return (dispatch) => {
            ApplicationService.updateStylesSettings(payload, data => {
                dispatch({
                    type: ActionTypes.UPDATED_STYLES,
                    data: {
                        clientModified: false
                    }
                });
                if (callback) {
                    callback(data);
                }
            }, data => {
                if (failureCallback) {
                    failureCallback(data);
                }
            });
        };
    },
    stylesClientModified(parameter) {
        return (dispatch) => {
            dispatch({
                type: ActionTypes.STYLES_CLIENT_MODIFIED,
                data: {
                    styles: parameter,
                    clientModified: true
                }
            });
        };
    }
};

export default stylesActions;