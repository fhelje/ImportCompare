import React, { useState, useEffect } from "react";
import { Typography, Grid } from "@material-ui/core";
import axios from "axios";
import { find, sortBy, differenceBy, each } from "lodash";
import { withStyles } from "@material-ui/core/styles";
import EnvironmentInfo from "./components/EnvironmentInfo";
import Details from "./components/Details";
const styles = theme => ({
  root: {
    ...theme.mixins.gutters(),
    paddingTop: theme.spacing.unit * 2,
    paddingBottom: theme.spacing.unit * 2,
  },
});

const LogDetail = ({ match }) => {
  const [id1] = useState(match.params.id1 ? parseInt(match.params.id1) : 0);
  const [id2] = useState(match.params.id2 ? parseInt(match.params.id2) : 0);
  const [data, setData] = useState({ items: { item1: [], item2: [], detail: [] } });
  const environments = ["VSPerfTest", "VSPerfTest2"];
  const fetchData = async () => {
    const [result1, result2, detail1, detail2] = await Promise.all([
      axios(`/api/importlog/${environments[0]}/${id1}`),
      axios(`/api/importlog/${environments[1]}/${id2}`),
      axios(`/api/importlog/detail/${environments[0]}/${id1}`),
      axios(`/api/importlog/detail/${environments[1]}/${id2}`),
    ]);
    var combined = detail1.data.items.map(o1 => {
      const o2 = find(detail2.data.items, x => x.eventTypeID === o1.eventTypeID);
      const { numberOfRows, ...retVal } = o1;
      return { ...retVal, numberOfRows1: o1.numberOfRows, numberOfRows2: (o2 && o2.numberOfRows) || 0 };
    });
    var diff = differenceBy(detail1.data.items, detail2.data.items, "eventTypeID");
    each(diff, item => {
      const { numberOfRows, ...rest } = item;
      combined.push({ numberOfRows1: numberOfRows, numberOfRows2: 0, ...rest });
    });
    setData({
      items: {
        item1: result1.data.items,
        item2: result2.data.items,
        detail: sortBy(combined, "eventTypeID"),
      },
    });
  };

  useEffect(() => {
    fetchData();
  }, []);
  return (
    <>
      <Typography variant="h2">
        {id1} - {id2}
      </Typography>
      <Grid container spacing={24}>
        <Details detail={data.items.detail} />
        <Grid item xs={12}>
          <Typography variant="h4">Files overview</Typography>
        </Grid>
        <EnvironmentInfo key={environments[0]} environment={environments[0]} items={data.items.item1 || []} />
        <EnvironmentInfo key={environments[1]} environment={environments[1]} items={data.items.item2 || []} />
      </Grid>
    </>
  );
};

export default withStyles(styles)(LogDetail);
