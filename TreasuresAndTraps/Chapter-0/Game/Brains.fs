namespace Game

open System
open Game.Domain

module Brains =

(* 
TODO: implement random decision making 
The Creature can go in either direction, 
with equal probability.
*)
    let r = new Random()

    let randomDecide () = 
        let direction = r.Next(3)
        match direction with
        | 0 -> Straight
        | 1 -> Right
        | _ -> Left
