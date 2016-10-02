﻿namespace DotNetLab.Fs.Lib.PFDS

open System
open Persimmon
open Persimmon.Syntax.UseTestNameByReflection
open DotNetLab.Fs.Lib.PFDS.Chapter03

module Chapter03Test =
  let testHeap (heapSig: HeapSignature<_, _>) =
    let empty = heapSig.Empty
    let ofList xs =
      heapSig.OfSeq(xs :> seq<_>)
    let insertRange xs heap =
      xs |> ofList |> heapSig.Merge heap
    let rec toList heap =
      [
        match heap |> heapSig.DeleteMin with
        | Some (x, heap) ->
          yield x
          yield! heap |> toList
        | None ->
          ()
      ]
    [
      yield
        test {
          do! empty |> heapSig.FindMin |> assertEquals None
          do! empty |> heapSig.DeleteMin |> assertEquals None
        }
      yield 
        test {
          let heap = empty |> insertRange [3; 1; 2]
          do! heap |> heapSig.FindMin |> assertEquals (Some 1)
        }
      yield
        test {
          let heap = empty |> insertRange [3; 1; 2]
          do! heap |> toList |> assertEquals [1; 2; 3]
        }
      yield
        test {
          do! [] |> ofList |> toList |> assertEquals []
        }
      yield
        test {
          do! [3; 1; 2] |> ofList |> toList |> assertEquals [1; 2; 3]
        }
    ]

  let testLeftistHeap =
    testHeap LeftistHeap<int>.AsHeap
