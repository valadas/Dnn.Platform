import React, { Component } from "react";
import PropTypes from "prop-types";
import { connect } from "react-redux";
import stylesActions from "../../actions/styles";
import resx from "../../resources";
import StylesColorVariant from "../stylesColorVariant";

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
            this.setState({...this.state, portalId: this.props.portalId});
            this.loadData();
        }
    }

    loadData() {
        this.props.dispatch(stylesActions.getPortalStyles(this.props.portalId, (data) => {
            this.setState({ ...this.state,
                styles: Object.assign({}, data.Settings.Styles)
            });
        }));
    }

    saveData() {
        this.props.dispatch(stylesActions.updatePortalStyles({ 
            PortalId: this.state.portalId,
            PrimaryColor: this.state.styles.PrimaryColor.HexValue,
            PrimaryColorContrast: this.state.styles.PrimaryColorContrast.HexValue,
            PrimaryColorDark: this.state.styles.PrimaryColorDark.HexValue,
            PrimaryColorLight: this.state.styles.PrimaryColorLight.HexValue,
            SecondaryColor: this.state.styles.SecondaryColor.HexValue,
            SecondaryColorContrast: this.state.styles.SecondaryColorContrast.HexValue,
            SecondaryColorDark: this.state.styles.SecondaryColorDark.HexValue,
            SecondaryColorLight: this.state.styles.SecondaryColorLight.HexValue,
            TertiaryColor: this.state.styles.TertiaryColor.HexValue,
            TertiaryColorContrast: this.state.styles.TertiaryColorContrast.HexValue,
            TertiaryColorDark: this.state.styles.TertiaryColorLight.HexValue,
            TertiaryColorLight: this.state.styles.TertiaryColorLight.HexValue,
            ControlsRadius: this.state.styles.ControlsRadius
        }));
    }

    render() {
        return (
            <div className="styles-color">
                {this.state.styles.PrimaryColor && 
                    <div>
                        <StylesColorVariant 
                            color={this.state.styles.PrimaryColor.HexValue}
                            colorClass="primary"
                            colorVariation="normal"
                            title={resx.get("PrimaryColor")}
                            onChanged={e => this.setState({...this.state, 
                                styles: {...this.state.styles, 
                                    PrimaryColor: {...this.state.styles.PrimaryColor,
                                        HexValue: e}}})}
                        />
                        <StylesColorVariant
                            color={this.state.styles.PrimaryColorLight.HexValue}
                            colorClass="primary"
                            colorVariation="light"
                            title={resx.get("PrimaryColorLight")} 
                            onChanged={e => this.setState({...this.state, 
                                styles: {...this.state.styles, 
                                    PrimaryColorLight: {...this.state.styles.PrimaryColorLight,
                                        HexValue: e}}})}
                        />
                        <StylesColorVariant
                            color={this.state.styles.PrimaryColorDark.HexValue}
                            colorClass="primary"
                            colorVariation="dark"
                            title={resx.get("PrimaryColorDark")} 
                            onChanged={e => this.setState({...this.state, 
                                styles: {...this.state.styles, 
                                    PrimaryColorDark: {...this.state.styles.PrimaryColorDark,
                                        HexValue: e}}})}
                        />
                        <StylesColorVariant
                            color={this.state.styles.PrimaryColorContrast.HexValue}
                            colorClass="primary"
                            colorVariation="contrast"
                            title={resx.get("PrimaryColorContrast")}
                            onChanged={e => this.setState({...this.state, 
                                styles: {...this.state.styles, 
                                    PrimaryColorContrast: {...this.state.styles.PrimaryColorContrast,
                                        HexValue: e}}})}
                        />
                        
                        <hr />

                        <StylesColorVariant
                            color={this.state.styles.SecondaryColor.HexValue}
                            colorClass="secondary"
                            colorVariation="normal"
                            title={resx.get("SecondaryColor")}
                            onChanged={e => this.setState({...this.state, 
                                styles: {...this.state.styles, 
                                    SecondaryColor: {...this.state.styles.SecondaryColor,
                                        HexValue: e}}})}
                        />
                        <StylesColorVariant
                            color={this.state.styles.SecondaryColorLight.HexValue}
                            colorClass="secondary"
                            colorVariation="light"
                            title={resx.get("SecondaryColorLight")}
                            onChanged={e => this.setState({...this.state, 
                                styles: {...this.state.styles, 
                                    SecondaryColorLight: {...this.state.styles.SecondaryColorLight,
                                        HexValue: e}}})}
                        />
                        <StylesColorVariant
                            color={this.state.styles.SecondaryColorDark.HexValue}
                            colorClass="secondary"
                            colorVariation="dark"
                            title={resx.get("SecondaryColorDark")}
                            onChanged={e => this.setState({...this.state, 
                                styles: {...this.state.styles, 
                                    SecondaryColorDark: {...this.state.styles.SecondaryColorDark,
                                        HexValue: e}}})}
                        />
                        <StylesColorVariant
                            color={this.state.styles.SecondaryColorContrast.HexValue}
                            colorClass="secondary"
                            colorVariation="contrast"
                            title={resx.get("SecondaryColorContrast")}
                            onChanged={e => this.setState({...this.state, 
                                styles: {...this.state.styles, 
                                    SecondaryColorContrast: {...this.state.styles.SecondaryColorContrast,
                                        HexValue: e}}})}
                        />

                        <hr />

                        <StylesColorVariant
                            color={this.state.styles.TertiaryColor.HexValue}
                            colorClass="tertiary"
                            colorVariation="normal"
                            title={resx.get("TertiaryColor")}
                            onChanged={e => this.setState({...this.state, 
                                styles: {...this.state.styles, 
                                    TertiaryColor: {...this.state.styles.TertiaryColor,
                                        HexValue: e}}})}
                        />
                        <StylesColorVariant
                            color={this.state.styles.TertiaryColorLight.HexValue}
                            colorClass="tertiary"
                            colorVariation="light"
                            title={resx.get("TertiaryColorLight")}
                            onChanged={e => this.setState({...this.state, 
                                styles: {...this.state.styles, 
                                    TertiaryColorLight: {...this.state.styles.TertiaryColorLight,
                                        HexValue: e}}})}
                        />
                        <StylesColorVariant
                            color={this.state.styles.TertiaryColorDark.HexValue}
                            colorClass="tertiary"
                            colorVariation="dark"
                            title={resx.get("TertiaryColorDark")}
                            onChanged={e => this.setState({...this.state, 
                                styles: {...this.state.styles, 
                                    TertiaryColorDark: {...this.state.styles.TertiaryColorDark,
                                        HexValue: e}}})}
                        />
                        <StylesColorVariant
                            color={this.state.styles.TertiaryColorContrast.HexValue}
                            colorClass="tertiary"
                            colorVariation="contrast"
                            title={resx.get("TertiaryColorContrast")}
                            onChanged={e => this.setState({...this.state, 
                                styles: {...this.state.styles, 
                                    TertiaryColorContrast: {...this.state.styles.TertiaryColorContrast,
                                        HexValue: e}}})}
                        />
                        <div style={{
                            borderStyle: "solid",
                            borderWidth: 1,
                            borderRadius: "var(--dnn-controls-radius)",
                            margin: 10,
                            padding: 10,
                            display: "flex",
                            alignItems: "center"
                        }}>
                            <label for="dnnControlsRadius" style={{marginRight: 10}}>{resx.get("ControlsRadius")} : </label>
                            <input type="range" id="dnnControlsRadius"
                                min={0} max={30} step={1}
                                value={this.state.styles.ControlsRadius}
                                onChange={e => {
                                    this.setState({...this.state, styles: { ...this.state.styles, ControlsRadius: e.target.value }});
                                    document.documentElement.style.setProperty("--dnn-controls-radius", `${this.state.styles.ControlsRadius}px`);
                                }}
                            ></input>
                            {this.state.styles.ControlsRadius}
                        </div>
                        <div style={{ display: "flex", justifyContent: "center"}}>
                            <dnn-button type="secondary" style={{ margin: 10 }} onClick={() => window.location.reload()}>{resx.get("Cancel")}</dnn-button>
                            <dnn-button size="large" style={{ margin: 10 }} onClick={() => this.saveData()}>{resx.get("Save")}</dnn-button>
                        </div>
                    </div>
                }
            </div>
        );
    }
}

StylesColor.propTypes = {
    dispatch: PropTypes.func.isRequired,
    styles: PropTypes.object,
    portalId: PropTypes.number,
    colorClass: PropTypes.oneOf(["primary", "secondary", "tertiary"]).isRequired
};

function mapStateToProps(state) {
    return {
        styles: state.styles.styles,
        portalId: state.siteInfo ? state.siteInfo.portalId : undefined
    };
}

export default connect(mapStateToProps)(StylesColor);