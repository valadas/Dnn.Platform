import React, { Component } from "react";
import PropTypes from "prop-types";
import { connect } from "react-redux";
import stylesActions from "../../actions/styles";

class StylesColor extends Component {
    constructor() {
        super();
        this.state = {
            portalId: undefined,
            styles: {}
        };
    }

    componentDidMount() {
        this.loadData();
    }

    componentDidUpdate() {
        let portalIdChanged = false;
        if (this.props.portalId === undefined || this.props.portalId === this.state.portalId) {
            portalIdChanged = false;
        }
        else {
            portalIdChanged = true;
        }
        if (portalIdChanged) {
            this.loadData();
        }
    }

    loadData() {
        this.props.dispatch(stylesActions.getPortalStyles(this.props.portalId, (data) => {
            this.setState({ ...this.state,
                styles: Object.assign({}, data.Styles)
            });
        }));
    }

    render() {
        return (
            <div className="styles-color">
                <p>Hello from React!</p>
            </div>
        );
    }
}

StylesColor.propTypes = {
    dispatch: PropTypes.func.isRequired,
    styles: PropTypes.object,
    portalId: PropTypes.number
};

function mapStateToProps(state) {
    return {
        styles: state.styles.styles,
        portalId: state.siteInfo ? state.siteInfo.portalId : undefined
    };
}

export default connect(mapStateToProps)(StylesColor);