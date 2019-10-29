import React from "react";
import { ChartBar } from "./ChartBar";
import CountiesSelect from "./CountiesSelect";
import { FormGroup, Col, Button } from "reactstrap";
import * as signalR from "@aspnet/signalr";

export const ChartContainer = () => {
  const [showAll, toggleShowAll] = React.useState(false);
  const [candidates, setCandidates] = React.useState(null);
  React.useEffect(() => {
     fetch(
       "/api/results"
     )
       .then(data => data.json())
       .then(data => {
         setCandidates(data.candidates);
       });
  }, []);
  const connection = new signalR.HubConnectionBuilder()
      .withUrl("/live-results")
      .build();

      connection
          .start()
          .then(() => console.log('Connection started!'))
          .catch(err => console.log('Error while establishing connection :('));

      connection.on('results-updated', (data) => {
          setCandidates(data.candidates);
      });
  return (
    <div>
      {candidates ? (
        <div>
          <div sm={12} className={"votes-container"}>
            <p className={"container-text"}>
              Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do
              eiusmod tempor incididunt ut labore et dolore magna aliqua.
              Viverra nam libero justo laoreet sit amet cursus sit. Malesuada
              nunc vel risus commodo viverra maecenas.
            </p>
            <div sm={3} className={"votes-numbers"}>
              <h3 className={"votes-title"}>Voturi numarate</h3>
              <div sm={3} className={"votes-results"}>
                <p className={"votes-percent"}>30%</p>
                <p className={"votes-text"}>12.400.000</p>
              </div>
            </div>
          </div>
          <FormGroup row>
            <Col sm={3}>
              <CountiesSelect />
            </Col>
          </FormGroup>
          {(showAll ? candidates : candidates.slice(0, 5)).map(candidate => (
            <ChartBar
              key={candidate.id}
              percent={candidate.percentage}
              imgUrl={candidate.imageUrl}
              candidateName={candidate.name}
              votesNumber={candidate.votes}
            />
          ))}
          {!showAll ? (
            <div className={"show-all-container"} sm={3}>
              <Button className={"show-all-btn"} onClick={toggleShowAll}>
                Afiseaza toti candidatii
              </Button>
            </div>
          ) : null}
        </div>
      ) : (
        <div className={"default-container"}>
          <div className={"votes-container"}>
            <p className={"container-text"}>
              Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do
              eiusmod tempor incididunt ut labore et dolore magna aliqua.
              Viverra nam libero justo laoreet sit amet cursus sit. Malesuada
              nunc vel risus commodo viverra maecenas.
            </p>
          </div>
          <div className={"question"}>
            <p className={"question-text"}>
              Cine sunt candidatii care merg in turul 2?
            </p>
          </div>
        </div>
      )}
    </div>
  );
};
