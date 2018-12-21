import React, { Component } from "react";
import { withStyles } from "@material-ui/core/styles";
import withRoot from "./components/withRoot";
import HomePage from "./pages/home/home";
import "./App.css";

import { BrowserRouter as Router, Route, Link } from "react-router-dom";
import { Grid, AppBar, Button, Toolbar } from "@material-ui/core";
import LogDetail from "./pages/log_detail/log-detail";

const styles = theme => ({
  root: {
    textAlign: "center",
    paddingTop: theme.spacing.unit * 2,
    paddingLeft: theme.spacing.unit * 2,
    paddingRight: theme.spacing.unit * 2,
  },
});

const HomeLink = props => <Link to="/" {...props} />;

class App extends Component {
  render() {
    return (
      <Router>
        <Grid container spacing={24}>
          <Grid item xs={12}>
            <AppBar>
              <Toolbar>
                <Button color="inherit" component={HomeLink}>
                  Home
                </Button>
              </Toolbar>
            </AppBar>
          </Grid>
          <Grid item xs={12}>
            <div className="main-page">
              <Route exact path="/" component={HomePage} />
              <Route exact path="/index.html" component={HomePage} />
              <Route exact path="/log-detail/:id1/:id2" component={LogDetail} />
            </div>
          </Grid>
        </Grid>
      </Router>
    );
  }
}

export default withRoot(withStyles(styles)(App));
