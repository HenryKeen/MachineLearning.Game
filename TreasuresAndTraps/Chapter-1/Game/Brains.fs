namespace Game

open System
open Game.Domain

module Brains =

(* random decision making *)

    let options = [| Straight; Left; Right; |]
    let rng = Random ()
    let randomDecide () = options.[rng.Next(options.Length)]

(* modelling state, strategy and brain *)

    type State = Dir * (Cell option) list

    type Experience = {
        State: State;
        Action: Act;
        Reward: float;
        NextState: State; }

    type Strategy = { State:State; Action:Act; }

    type Brain = Map<Strategy,float>

(* What the creature "sees" *)

    let visibleBy (size:Size) (creature:Creature) (board:Board) =
        creature.Direction,
        [   for t in -1 .. 1 do
                for l in -1 .. 1 ->
                    board.[(creature.Position.Top + t) %%% size.Height, (creature.Position.Left + l) %%% size.Width]
        ]
                
(*
TODO: implement learn, so that the creature
updates its brain based on the latest experience it had.
- create the Strategy corresponding to the State and
Decision from the latest Experience recorded,
- update the Strategy value as 
(1 - alpha) * V(strat) + alpha * reward from experience 
*)
    let alpha = 0.2 // learning rate
    
    let learn (brain:Brain) (exp:Experience) =
        let strategy = { State = exp.State; Action = exp.Action; }

        let existingStrategy = brain.TryFind(strategy)
        
        let nextVal = match existingStrategy with
            | None -> 0.
            | Some(t) -> existingStrategy.Value 

        let strategyValue = (1. - alpha) * nextVal + alpha * exp.Reward
        brain.Add(strategy, strategyValue)


(*
TODO: implement decide, so that the creature
uses its brain to make a decision, based on its
current state.
- if the state has never been seen before, 
make a random move,
- otherwise, among the known strategies that 
correspond to the current state, pick the strategy 
that has the highest value.
*)
    let decide (brain:Brain) (state:State) =
        let existingState = brain
                            |> Seq.tryFind(fun x -> x.Key.State = state)
        match existingState with
        | None -> randomDecide()
        | Some(t) -> if existingState.Value.Value > 0. then existingState.Value.Key.Action else randomDecide()