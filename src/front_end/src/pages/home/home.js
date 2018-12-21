import React, { useState, useEffect } from "react";
import { useInterval } from "react-powerhooks";
import { withStyles } from "@material-ui/core/styles";
import { Grid, Typography, Button, Checkbox, FormGroup, FormControlLabel } from "@material-ui/core";
import axios from "axios";
import Logs from "./components/Logs";
import { find } from "lodash";
import "./home.css";

const styles = theme => ({
  loading: {
    marginLeft: theme.spacing.unit * 2,
    marginTop: theme.spacing.unit * 2 - 4,
  },
});

const HomePage = ({ classes }) => {
  const [timerToggle, setTimerToggle] = useState(true);
  const [data, setData] = useState({ items: [] });
  const [isLoading, setIsLoading] = useState(false);

  const { start, stop } = useInterval({
    duration: 10000,
    startImmediate: timerToggle,
    callback: () => {
      console.log("Refresh");
      fetchData();
    },
  });

  const fetchData = async () => {
    setIsLoading(true);
    const [result1, result2] = await Promise.all([
      axios("/api/importlog/VSPerfTest"),
      axios("/api/importlog/VSPerfTest2"),
    ]);
    var combined = result1.data.items.map(o1 => {
      const o2 = find(result2.data.items, x => x.id === o1.id);
      return {
        VSPerfTest: o1,
        VSPerfTest2: o2,
      };
    });
    setData({ items: combined });
    setIsLoading(false);
  };

  useEffect(() => {
    fetchData();
  }, []);

  const handleAutorefresh = () => {
    var newValue = !timerToggle;
    setTimerToggle(newValue);
    if (newValue) {
      fetchData();
      start();
    } else {
      stop();
    }
  };
  return (
    <Grid>
      <Typography variant="h2">Import logs</Typography>
      <div className="toolbar">
        <FormGroup row>
          <FormControlLabel
            control={<Checkbox checked={timerToggle} onChange={handleAutorefresh} value="checkedA" />}
            label="Autorefresh"
          />
          <Button variant="contained" color="primary" onClick={() => fetchData()}>
            Refresh
          </Button>
          {isLoading && (
            <Typography className={classes.loading} variant="subtitle1">
              Loading ...
            </Typography>
          )}
        </FormGroup>
      </div>

      <Logs rows={data.items} />
    </Grid>
  );
};

export default withStyles(styles)(HomePage);
