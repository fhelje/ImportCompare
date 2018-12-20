import React, { useState, useEffect } from "react";
import { useInterval } from "react-powerhooks";
import { Grid, Typography } from "@material-ui/core";
import axios from "axios";
import Logs from "./components/Logs";
import { find } from "lodash";
const HomePage = () => {
  const [data, setData] = useState({ items: [] });
  const [time, setTime] = useState(null);

  const { start, stop } = useInterval({
    duration: 10000,
    startImmediate: true,
    callback: () => {
      console.log("Refresh");
      fetchData();
    },
  });

  const fetchData = async () => {
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
  };

  useEffect(() => {
    fetchData();
  }, []);
  return (
    <Grid>
      <Typography variant="h2">Import logs</Typography>
      <Logs rows={data.items} />
    </Grid>
  );
};

export default HomePage;
