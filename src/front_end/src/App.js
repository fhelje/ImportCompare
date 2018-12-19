import React, { Component } from "react";
import { withStyles } from "@material-ui/core/styles";
import withRoot from "./components/withRoot";
import HomePage from "./pages/home";

import { BrowserRouter as Router, Route, Link } from "react-router-dom";
import { Grid } from "@material-ui/core";

const styles = theme => ({
  root: {
    textAlign: "center",
    paddingTop: theme.spacing.unit * 2,
    paddingLeft: theme.spacing.unit * 2,
    paddingRight: theme.spacing.unit * 2,
  },
});

class App extends Component {
  render() {
    return (
      <Router>
        <Grid container spacing={24}>
          <Grid item xs={12}>
            <div>
              <Route exact path="/" component={HomePage} />
            </div>
          </Grid>
        </Grid>
      </Router>
    );
  }
}

export default withRoot(withStyles(styles)(App));
