import React, { Component, createRef } from "react";
import PropTypes from "prop-types";

class StylesColorVariant extends Component {

    constructor() {
        super();
        this.state = {
            color: "000000"
        };
        this.normalChevron = createRef();
        this.normalPanel = createRef();
        this.picker = createRef();
    }

    componentDidMount() {
        this.normalPanel.current.expanded = false;
        this.normalChevron.current.addEventListener("changed", e => {
            this.normalPanel.current.expanded = e.detail;
        });
        this.setState({...this.state, color: this.props.color});
        this.picker.current.addEventListener("colorChanged", e => {
            this.setState({...this.state, color: e.detail});
            document.documentElement.style.setProperty(this.getCssVarName(), `#${e.detail.hex}`);
            this.props.onChanged(e.detail.hex);
        });
    }

    getCssVarName() {
        let cssVar = `--dnn-color-${this.props.colorClass}`;
        switch (this.props.colorVariation) {
            case "dark":
                cssVar += "-dark";
                break;
            case "light":
                cssVar += "-light";
                break;
            case "contrast":
                cssVar += "-contrast";
                break;
            default:
                break;
        }
        return cssVar;
    }

    getBackgroundColor() {
        return (`var(${this.getCssVarName()})`);
    }

    getForegroundColor() {
        if (this.props.colorVariation === "contrast") {
            return `var(--dnn-color-${this.props.colorClass})`;
        }
        else {
            return `var(--dnn-color-${this.props.colorClass}-contrast)`;
        }
    }

    render() {
        return (
            <div className="color-container" style={{
                margin: 15,
                border: "1px solid lightgray"
            }}>
                <div className="color-group" style={{
                    display: "flex",
                    alignItems: "center"
                }}>
                    <dnn-chevron ref={this.normalChevron}/>
                    <div className="color-title" style={{
                        width: "100%",
                        padding: "5px 15px",
                        backgroundColor: this.getBackgroundColor(),
                        color: this.getForegroundColor(),
                        fontWeight: "bold"
                    }}>
                        {this.props.title}
                    </div>
                </div>
                <dnn-collapsible ref={this.normalPanel}>
                    <dnn-color-picker ref={this.picker} color={this.state.color} style={{margin: "0 auto"}} />
                </dnn-collapsible>
            </div>
        );
    }
}

StylesColorVariant.propTypes = {
    color: PropTypes.string.isRequired,
    colorClass: PropTypes.oneOf(["primary", "secondary", "tertiary"]).isRequired,
    colorVariation: PropTypes.oneOf(["normal", "dark", "light", "contrast"]).isRequired,
    title: PropTypes.string.isRequired,
    onChanged: PropTypes.func.isRequired
};

export default StylesColorVariant;