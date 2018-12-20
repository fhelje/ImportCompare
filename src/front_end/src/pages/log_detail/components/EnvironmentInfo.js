import React from "react";
import { Typography, Paper, Grid } from "@material-ui/core";
import { withStyles } from "@material-ui/core/styles";

const styles = theme => ({
  root: {
    ...theme.mixins.gutters(),
    paddingTop: theme.spacing.unit * 2,
    paddingBottom: theme.spacing.unit * 2,
  },
});

const EnvironmentInfo = ({ environment, classes, items }) => {
  var rows = items || [];
  return (
    <Grid item xs={6}>
      <Paper className={classes.root}>
        <Typography variant="h5">{environment}</Typography>
        {rows.map(x => (
          <div key={x.id}>
            <Typography variant="h6">{x.fileName}</Typography>
            <div>
              <dl>
                <dt>Number Of Rows</dt>
                <dd>{x.numberOfRows}</dd>

                <dt>Number Of Commands</dt>
                <dd>{x.numberOfCommands}</dd>

                <dt>Number Of Errors</dt>
                <dd>{x.numberOfErrors}</dd>

                <dt>Number Of Duplicates</dt>
                <dd>{x.numberOfDuplicates}</dd>
              </dl>
            </div>
          </div>
        ))}
      </Paper>
    </Grid>
  );
};

export default withStyles(styles)(EnvironmentInfo);
