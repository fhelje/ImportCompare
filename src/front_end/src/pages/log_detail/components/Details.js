import React from "react";
import PropTypes from "prop-types";
import { withStyles } from "@material-ui/core/styles";
import { Table, TableBody, TableCell, TableHead, TableRow, Paper, Typography } from "@material-ui/core";
import blue from "@material-ui/core/colors/blue";

const CustomTableCell = withStyles(theme => ({
  head: {
    backgroundColor: blue[500],
    color: theme.palette.common.white,
  },
  body: {
    fontSize: 14,
  },
}))(TableCell);

const styles = theme => ({
  root: {
    width: "100%",
    marginTop: theme.spacing.unit * 3,
    overflowX: "auto",
  },
  table: {
    minWidth: 700,
  },
  row: {
    "&:nth-of-type(odd)": {
      backgroundColor: theme.palette.background.default,
    },
  },
});

const ColoredNotZero = ({ value }) => {
  const color = value !== 0 ? "error" : "secondary";
  return (
    <Typography variant="caption" color={color}>
      {value}
    </Typography>
  );
};

const Details = ({ classes, detail }) => {
  return (
    <Paper className={classes.root}>
      <Table className={classes.table}>
        <TableHead>
          <TableRow>
            <CustomTableCell>Description</CustomTableCell>
            <CustomTableCell>EventTypeID</CustomTableCell>
            <CustomTableCell>NumberOfRows1</CustomTableCell>
            <CustomTableCell>NumberOfRows2</CustomTableCell>
            <CustomTableCell>Diff</CustomTableCell>
            <CustomTableCell>TodoLogDetailID</CustomTableCell>
            <CustomTableCell>TodoLogID</CustomTableCell>
          </TableRow>
        </TableHead>
        <TableBody>
          {detail.map(x => {
            return (
              <TableRow key={x.eventTypeID}>
                <CustomTableCell>{x.description}</CustomTableCell>
                <CustomTableCell>{x.eventTypeID}</CustomTableCell>
                <CustomTableCell>{x.numberOfRows1}</CustomTableCell>
                <CustomTableCell>{x.numberOfRows2}</CustomTableCell>
                <CustomTableCell>
                  <ColoredNotZero value={x.numberOfRows1 - x.numberOfRows2} />
                </CustomTableCell>
                <CustomTableCell>{x.todoLogDetailID}</CustomTableCell>
                <CustomTableCell>{x.todoLogID}</CustomTableCell>
              </TableRow>
            );
          })}
        </TableBody>
      </Table>
    </Paper>
  );
};

Details.propTypes = {
  classes: PropTypes.object.isRequired,
};

export default withStyles(styles)(Details);
