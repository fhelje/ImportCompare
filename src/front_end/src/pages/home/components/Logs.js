import React from "react";
import PropTypes from "prop-types";
import { withStyles } from "@material-ui/core/styles";
import { Table, TableBody, TableCell, TableHead, TableRow, Paper, Button, Typography } from "@material-ui/core";
import { Duration, DateTime } from "luxon";
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

// const hasSameEvents = (r1, r2) => {
//   return (
//     r1.numberOfRows === r2.numberOfRows &&
//     r1.numberOfCommands === r2.numberOfCommands &&
//     r1.numberOfErrors === r2.numberOfErrors &&
//     r1.numberOfDuplicates === r2.numberOfDuplicates
//   );
// };

const DubbleCell = ({ value1, value2, showDiff }) => {
  const presentDiff = showDiff || false;

  const color = presentDiff ? (value1 !== value2 ? "error" : "default") : "default";
  return (
    <>
      <Typography variant="caption" color={color}>
        {value1}
      </Typography>
      <Typography variant="caption" color={color}>
        {value2}
      </Typography>
    </>
  );
};

const DubbleCellDuration = ({ value1, value2, showDiff }) => {
  const color = value1 > value2 ? "error" : "secondary";
  const percentage = (value2.milliseconds - value1.milliseconds) / value2.milliseconds;

  return (
    <>
      <Typography variant="caption" color={color}>
        {value1.toFormat("hh:mm:ss.SSS")} ({(percentage * 100).toFixed(0)}%)
      </Typography>
      <Typography variant="caption" color="default">
        {value2.toFormat("hh:mm:ss.SSS")}
      </Typography>
    </>
  );
};

const SingleCell = ({ value }) => {
  return <Typography variant="caption">{value}</Typography>;
};

const toTime = val => DateTime.fromISO(val).toFormat("HH:mm:ss");
const toDate = val => DateTime.fromISO(val).toLocaleString();
const toDuration1 = val => Duration.fromMillis(val);

const logMissing = (row1, row2) => {
  console.log(row1);
  console.log(row2);
  return null;
};

const Logs = ({ classes, rows }) => {
  return (
    <Paper className={classes.root}>
      <Table className={classes.table}>
        <TableHead>
          <TableRow>
            <CustomTableCell>Env</CustomTableCell>
            <CustomTableCell>Date</CustomTableCell>
            <CustomTableCell>Start/End</CustomTableCell>
            <CustomTableCell>Duration</CustomTableCell>
            <CustomTableCell>Partner</CustomTableCell>
            <CustomTableCell>TodoID</CustomTableCell>
            <CustomTableCell>File Name</CustomTableCell>
            {/* <CustomTableCell>Message ID</CustomTableCell> */}
            <CustomTableCell>Rows</CustomTableCell>
            <CustomTableCell>Commands</CustomTableCell>
            <CustomTableCell>Errors</CustomTableCell>
            <CustomTableCell>Duplicates</CustomTableCell>
            <CustomTableCell>Events</CustomTableCell>
            <CustomTableCell>Details</CustomTableCell>
          </TableRow>
        </TableHead>
        <TableBody>
          {rows.map(rowPair => {
            const row1 = rowPair["VSPerfTest"];
            const row2 = rowPair["VSPerfTest2"];
            return row1 && row2 ? (
              <TableRow key={row1.id}>
                <CustomTableCell>
                  <DubbleCell value1="New" value2="Old" />
                </CustomTableCell>
                <CustomTableCell>
                  <DubbleCell value1={toDate(row1.startDate)} value2={toDate(row2.startDate)} />
                </CustomTableCell>
                <CustomTableCell>
                  <DubbleCell
                    value1={`${toTime(row1.startDate)} - ${toTime(row1.endDate)}`}
                    value2={`${toTime(row2.startDate)} - ${toTime(row2.endDate)}`}
                  />
                </CustomTableCell>
                <CustomTableCell>
                  <DubbleCellDuration value1={toDuration1(row1.duration)} value2={toDuration1(row2.duration)} />
                </CustomTableCell>
                <CustomTableCell>
                  <SingleCell value={row1.partnerName} />
                </CustomTableCell>
                <CustomTableCell>
                  <DubbleCell value1={row1.todoID} value2={row2.todoID} />
                </CustomTableCell>
                <CustomTableCell>
                  <SingleCell value={row1.fileName} />
                </CustomTableCell>
                {/* <CustomTableCell>{row.messageID}</CustomTableCell> */}
                <CustomTableCell>
                  <DubbleCell value1={row1.numberOfRows} value2={row2.numberOfRows} showDiff={true} />
                </CustomTableCell>
                <CustomTableCell>
                  <DubbleCell value1={row1.numberOfCommands} value2={row2.numberOfCommands} showDiff={true} />
                </CustomTableCell>
                <CustomTableCell>
                  <DubbleCell value1={row1.numberOfErrors} value2={row2.numberOfErrors} showDiff={true} />
                </CustomTableCell>
                <CustomTableCell>
                  <DubbleCell value1={row1.numberOfDuplicates} value2={row2.numberOfDuplicates} showDiff={true} />
                </CustomTableCell>
                <CustomTableCell>
                  <DubbleCell value1={row1.totalNumberOfEvents} value2={row2.totalNumberOfEvents} showDiff={true} />
                </CustomTableCell>
                <CustomTableCell>
                  <Button href={`/log-detail/${row1.todoID}/${row2.todoID}`}>Show</Button>
                </CustomTableCell>
              </TableRow>
            ) : (
              logMissing(row1, row2)
            );
          })}
        </TableBody>
      </Table>
    </Paper>
  );
};

Logs.propTypes = {
  classes: PropTypes.object.isRequired,
};

export default withStyles(styles)(Logs);
